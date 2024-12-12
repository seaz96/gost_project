// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const BackendErrorParser = (error: any) => {
  if(error.data && error.data.field) {
    return [error.data.field.toLowerCase()]
  }
}