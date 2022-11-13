interface JQuery {
  modal: (action: string) => void;
  valid: () => boolean;
}

interface JQueryStatic {
  validator: {
    defaults: unknown
    setDefaults: (options: unknown) => void;
    unobtrusive: {
      adapters: {
        addBool: (name: string, method: string) => void;
      };
      options: unknown;
      parse: (form: HTMLFormElement) => void;
    }
  };
}
