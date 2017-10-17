export class DataTableViewModel {
  public recordsTotal: number;
  public recordsFiltered: number;
  public columns: string[];
  public rows: Array<DataItem>;
}

export class DataItem {
  public dataProperties: Array<DataProperty>;
}

export class DataProperty {
  public dataType: string;
  public dataContent: string;
}
