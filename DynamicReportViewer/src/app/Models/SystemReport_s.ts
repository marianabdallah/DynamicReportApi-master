export class SystemReport_s {
  id: number;
  reportTitle: string;
  isFunction: boolean;
  isActive: boolean;
}

export class InputObj {
  order: number;
  name: string;
  dataType: string;
  isList: boolean;
  showInput: boolean;
  listSource: Source[];
  value: any;
  isEnum: boolean;
  enumValues: EnumValues;
  dateRanges: string[];
}

export class Source {
  id: number;
  name: string;
}

export class GetSystemReportByIdResponse {
  reportTitle: string;
  inputStructure: InputObj[];
  outputStructure: OutputObj[];
}

export class OutputObj {
  order: number;
  name: string;
  dataType: string;
  showInput: boolean;
}

export class EnumValues {
  names: NameObj[];
}

export class NameObj {
  name: string;
  value: number;
}

export class ExecuteSystemReportByIdRequest {
  reportId: number;
  inputObject: InputExecute[];
}

export class InputExecute {
  order: number;
  name: string;
  dataType: string;
  value: any;
}

export class ExecuteSystemReportByIdResponse {
  reportTitle: string;
  outputStructure: OutputObj[];
  resultDataSet: any[];
}
