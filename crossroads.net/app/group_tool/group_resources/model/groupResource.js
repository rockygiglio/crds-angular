
export default class GroupResource {
  constructor(jsonObject) {
    if (jsonObject) {
      this._assignProperties(jsonObject);
    } else {
      this._assignProperties({});
    }
  }

  _assignProperties(source) {
      this.title = source.title;
      this.tagline = source.tagline;
      this.url = source.url;
      this.author = source.author;
      this.image = source.img;
      this.type = source.type;
      this.sortOrder = source.sortOrder;
  }

  getTitle() {
    return this.title;
  }

  getTagline() {
    return this.tagline;
  }

  getUrl() {
    return this.url;
  }

  hasUrl() {
    return this.url !== undefined && this.url !== null && this.url.length > 0;
  }

  getAuthor() {
    return this.author;
  }

  getImage() {
    return this.image;
  }

  getType() {
    return this.type;
  }

  getSortOrder() {
    return this.sortOrder;
  }

  compareTo(other) {
    if(this.getSortOrder() === undefined && other.getSortOrder() === undefined) {
      return 0;
    }

    if(this.getSortOrder() === undefined) {
      return -1;
    }

    if(other.getSortOrder() === undefined) {
      return 1;
    }

    let compare = this.getSortOrder() - other.getSortOrder();
    return compare > 0 ? 1 : compare < 0 ? -1 : 0; 
  }
}