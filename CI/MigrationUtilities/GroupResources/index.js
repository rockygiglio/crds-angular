var http = require('http');
var fs = require('fs');
var dateformat = require('dateformat');

var now = new Date();

// This gets invoked when the http.get() below executes
var main = function(response) {
  var content = '';

  response.on('data', function (chunk) {
    content += chunk;
  });

  response.on('end', function () {
    processCategories(JSON.parse(content).resources.category);
  });
};

// Process all the categories returned from the http.get() 
var processCategories = function(categories) {
  var sqlFileTemplate = fs.readFileSync('InsertGroupResourcesTemplate.sql', "utf8");
  var inserts = '';

  for(var i = 0; i < categories.length; i++) {
    inserts += '\n\n-- =============================================\n';
    inserts += '-- Category: ' + categories[i].title + '\n';
    inserts += '-- =============================================\n';
    inserts += "INSERT INTO SS_mysite.GroupResourceCategory (ClassName, Created, LastEdited, Title, `Default`, SortOrder, Description, FooterContent)\n";
    inserts += "  SELECT * FROM (SELECT 'GroupResourceCategory', CURDATE(), CURDATE() as d2, '" + 
      quote(categories[i].title) + "', " + 
      (categories[i].active === 'true' ? 1 : 0) + ", " + i + ", " + 
      "'" + quote(categories[i].description) + "', " +
      (categories[i].communitygroupcontent === undefined || categories[i].communitygroupcontent === null ? "NULL" : "'" + quote(categories[i].communitygroupcontent) + "'") + ")\n";
    inserts += "  AS tmp WHERE NOT EXISTS (SELECT Title FROM SS_mysite.GroupResourceCategory WHERE Title = '" + quote(categories[i].title) + "') LIMIT 1;\n";
    inserts += 'SELECT LAST_INSERT_ID() INTO `@catId`;\n';
    inserts += processResources(categories[i].resources);
  }

  fs.writeFile(dateformat(now, 'yyyymmdd_HHMMss') + '_USnnnn-InsertGroupResources.sql', sqlFileTemplate.replace('SQL_INSERTS', inserts));

  getFiles(categories);
};

// Process all the resources on a category returned from the http.get()
var processResources = function(resources) {
  var res = '';
  for(var i = 0; i < resources.length; i++) {
    var type = resources[i].type;
    var image = resources[i].img.split('/');
    var resClass = 'OtherGroupResource';
    if(type === 'file-pdf') {
      resClass = 'PdfGroupResource';
    } else if(type === 'book') {
      resClass = 'BookGroupResource';
    }

    res += '\n-- Resource: ' + resources[i].title + '\n';

    var url = type === 'file-pdf' ? "NULL" : "'" + resources[i].url + "'";
    res += "SELECT ID INTO `@imageId` FROM SS_mysite.File WHERE Filename LIKE '%groupresources/images/" + image[image.length - 1] + "' LIMIT 1;\n";
    res += "INSERT INTO SS_mysite.GroupResource (ClassName, Created, LastEdited, Title, Tagline, Url, Author, ResourceType, SortOrder, GroupResourceCategoryID, ImageID)\n";
    res += "  SELECT * FROM (SELECT '" + resClass + "', CURDATE(), CURDATE() as d2, '" + quote(resources[i].title) + "', '" + quote(resources[i].tagline) + "', " + 
      url + ", '" + quote(resources[i].author) + "', '" + type + "', " + i + ", `@catId`, `@imageId`)\n";
    res += "  AS tmp WHERE NOT EXISTS (SELECT Title FROM SS_mysite.GroupResource WHERE Title = '" + quote(resources[i].title) + "' AND GroupResourceCategoryID = `@catId`) LIMIT 1;\n";

    if(type !== 'file-pdf') {
      continue;
    }

    var pdf = resources[i].url.split('/');
    res += '\n-- PDF: ' + pdf[pdf.length - 1] + '\n';
    res += "SELECT ID INTO `@resourceId` FROM SS_mysite.GroupResource WHERE Title = '" + quote(resources[i].title) + "' AND GroupResourceCategoryID = `@catId` LIMIT 1;\n";
    res += "SELECT ID INTO `@pdfId` FROM SS_mysite.File WHERE Filename LIKE '%groupresources/pdf/" + pdf[pdf.length - 1] + "' LIMIT 1;\n";
    res += "INSERT INTO SS_mysite.PdfGroupResource (ID, PdfFileID) SELECT * FROM (SELECT `@resourceId`, `@pdfId`) AS tmp WHERE NOT EXISTS (SELECT ID FROM SS_mysite.PdfGroupResource WHERE ID = `@resourceId`) LIMIT 1;\n";
  }

  return res;
};

// Replace single quotes with two single quotes in SQL inserts
var quote = function(str) {
  return(str.replace(/'/g, "''"));
};

// Generate a shell script to download images and PDFs
var getFiles = function(categories) {
  var curl = '';
  curl += "# Get Group Resource images and PDFs\n";
  for(var i = 0; i < categories.length; i++) {
    for(var j = 0; j < categories[i].resources.length; j++) {
      curl += "curl -O " + categories[i].resources[j].img + '\n';
      if(categories[i].resources[j].type === 'file-pdf') {
        curl += "curl -O " + categories[i].resources[j].url + '\n';
      }
    }
  }

  fs.writeFile(dateformat(now, 'yyyymmdd_HHMMss') + '_USnnnn-DownloadGroupResources.sh', curl);
};

// Get the static "V1" Group Resources JSON
http.get('http://crossroads-media.s3.amazonaws.com/documents/group-resources/group-resources.json', main).on('error', function(e) {
  console.log('Got error: ' + JSON.stringify(e));
});