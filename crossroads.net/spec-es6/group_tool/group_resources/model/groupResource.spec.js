
import GroupResource from '../../../../app/group_tool/group_resources/model/groupResource';

describe('GroupResource model', () => {
  describe('the constructor', () => {
    it('should populate an empty GroupResource with no JSON', () => {
      let fixture = new GroupResource();
      expect(fixture.getTitle()).not.toBeDefined();
      expect(fixture.getTagline()).not.toBeDefined();
      expect(fixture.getUrl()).not.toBeDefined();
      expect(fixture.hasUrl()).toBeFalsy();
      expect(fixture.getAuthor()).not.toBeDefined();
      expect(fixture.getImage()).not.toBeDefined();
      expect(fixture.getType()).not.toBeDefined();
      expect(fixture.getSortOrder()).not.toBeDefined();
    });

    it('should populate GroupResource object with JSON', () => {
      let resource = {
        title: 'title1',
        tagline: 'tagline1',
        url: 'url1',
        author: 'author1',
        img: 'image1',
        type: 'type1',
        sortOrder: 1
      };

      let fixture = new GroupResource(resource);
      expect(fixture.getTitle()).toEqual(resource.title);
      expect(fixture.getTagline()).toEqual(resource.tagline);
      expect(fixture.getUrl()).toEqual(resource.url);
      expect(fixture.hasUrl()).toBeTruthy();
      expect(fixture.getAuthor()).toEqual(resource.author);
      expect(fixture.getImage()).toEqual(resource.img);
      expect(fixture.getType()).toEqual(resource.type);
      expect(fixture.getSortOrder()).toEqual(resource.sortOrder);
    });
  });

  describe('compareTo function', () => {
    it('should return + if other is undefined', () => {
      let r1 = new GroupResource({sortOrder: 1});
      let r2;
      expect(r1.compareTo(r2)).toEqual(1);
    });

    it('should return + if other is null', () => {
      let r1 = new GroupResource({sortOrder: 1});
      let r2 = null;
      expect(r1.compareTo(r2)).toEqual(1);
    });

    it('should return 0 if sort orders are undefined', () => {
      let r1 = new GroupResource();
      let r2 = new GroupResource();
      expect(r1.compareTo(r2)).toEqual(0);
    });
    
    it('should return - if this sort order is undefined', () => {
      let r1 = new GroupResource();
      let r2 = new GroupResource({sortOrder: 1});
      expect(r1.compareTo(r2)).toEqual(-1);
    });
    
    it('should return + if other sort order is undefined', () => {
      let r1 = new GroupResource({sortOrder: 1});
      let r2 = new GroupResource();
      expect(r1.compareTo(r2)).toEqual(1);
    });

    it('should return appropriately for sort orders', () => {
      let r1 = new GroupResource({sortOrder: 1});
      let r2 = new GroupResource({sortOrder: 1});
      expect(r1.compareTo(r2)).toEqual(0);

      r2.sortOrder = 2;
      expect(r1.compareTo(r2)).toEqual(-1);

      r1.sortOrder = 3;
      expect(r1.compareTo(r2)).toEqual(1);
    });
  });
});
