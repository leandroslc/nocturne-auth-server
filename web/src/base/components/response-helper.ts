type Response = {
  status: number;
  getResponseHeader: (name: string) => string | null;
};

export default () => {
  const isBadRequest = (response: Response) => {
    return response.status === 400;
  };

  const isHtml = (response: Response) => {
    const contentType = response.getResponseHeader('content-type');

    return contentType && contentType.indexOf('text/html') !== -1;
  };

  return {
    isBadRequest,
    isHtml,
  };
};
