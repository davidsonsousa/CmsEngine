export class Service {
  public extractProperties(item: any): any[] {
    let propList = [];

    if (item) {
      for (var prop in item) {
        if (item.hasOwnProperty(prop)) {
          propList.push(prop);
        }
      }
    }

    return propList;
  }
}