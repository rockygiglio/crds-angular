/* ngInject */
class SummercampService { 
  constructor($resource, Session) {
    this.resource = $resource;
    this.session = Session;
  }
}
export default SummercampService;