export interface ResponseModel<T> {
  flag: boolean;
  message: string;
  data: T;
}
