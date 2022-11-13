window.searchRoles = (options) => {
  const { searchForm: form, searchUrl, results } = options;
  const formLoading = window.Loading.create(form);

  const search = () => {
    formLoading.start();

    const data = $(form).serialize();

    $.get(searchUrl, data, (response) => {
      results.innerHTML = response;
      formLoading.stop();
    });
  };

  form.addEventListener('submit', (event) => {
    event.preventDefault();

    search();
  });

  return {
    search,
  };
};

window.manageRoles = (options) => {
  const PermissionActionAttribute = 'data-role-action';

  const searchPermissions = window.searchRoles(options);
  const { modalId, results } = options;

  const modal = new window.Modal(modalId);

  const createModalFormSubmitEvent = (form: HTMLFormElement, url: string) => {
    const formLoading = window.Loading.create(form);

    return (event: Event) => {
      event.preventDefault();

      if (!window.Validator.isValid(form)) {
        return;
      }

      formLoading.start();

      const data = $(form).serialize();

      $.post(url, data, () => {
        searchPermissions.search();
        modal.close();
      }).fail((response) => {
        if (
          window.ResponseHelper.isBadRequest(response) &&
          window.ResponseHelper.isHtml(response)
        ) {
          // eslint-disable-next-line no-use-before-define
          setupModal(response.responseText, url);
        }

        modal.close();
      });
    };
  };

  const setupModalForm = (url: string) => {
    const form = modal.querySelector('form') as HTMLFormElement;

    if (!form) {
      return;
    }

    window.Validator.attach(form);

    form.addEventListener('submit', createModalFormSubmitEvent(form, url));
  };

  const setupModal = (content: string, url: string) => {
    modal.setContent(content);
    setupModalForm(url);
  };

  const show = (element: Element) => {
    const url = element.getAttribute(PermissionActionAttribute)!;

    $.get(url, (response) => {
      setupModal(response, url);
      modal.show();
    });
  };

  results.addEventListener('click', (event) => {
    const target = event.target as HTMLElement;
    const element = target.closest(`[${PermissionActionAttribute}]`);

    if (!element) {
      return;
    }

    show(element);
  });

  searchPermissions.search();
};
