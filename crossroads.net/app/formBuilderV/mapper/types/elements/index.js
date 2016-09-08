export default ngModule => {
    require('./firstName')(ngModule);
    require('./lastName')(ngModule);
    require('./preferredName')(ngModule);
}