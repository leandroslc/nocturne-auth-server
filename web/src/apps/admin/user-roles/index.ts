window.searchUserRoles = (options) => {
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

window.manageUserRoles = (options) => {
  const RoleActionAttribute = 'data-role-action';
  const ApplicationChangeAttribute = 'data-application-change';

  const searchRoles = window.searchUserRoles(options);
  const { addRolesButton, modalId, results } = options;

  const modal = new window.Modal(modalId);

  function createModalFormSubmitEvent(form: HTMLFormElement, url: string) {
    const formLoading = window.Loading.create(form);

    return (event: Event) => {
      event.preventDefault();

      if (!window.Validator.isValid(form)) {
        return;
      }

      formLoading.start();

      const data = $(form).serialize();

      $.post(url, data, () => {
        searchRoles.search();
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
  }

  const createChangeApplicationEvent = (
    form: HTMLFormElement,
    baseApplicationChangeUrl: string,
  ) => {
    const formLoading = window.Loading.create(form);

    return (event: Event) => {
      event.preventDefault();

      const target = event.target as HTMLInputElement;
      const applicationId = target.value;
      const url = `${baseApplicationChangeUrl}?applicationId=${applicationId}`;

      formLoading.start();

      $.get(url, (response) => {
        // eslint-disable-next-line no-use-before-define
        setupModal(response, url);
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

    const changeApplicationElement = form.querySelector(
      `[${ApplicationChangeAttribute}]`,
    );

    if (changeApplicationElement) {
      const baseApplicationChangeUrl = changeApplicationElement.getAttribute(
        ApplicationChangeAttribute,
      )!;

      changeApplicationElement.addEventListener(
        'change',
        createChangeApplicationEvent(form, baseApplicationChangeUrl),
      );
    }
  };

  const setupModal = (content: string, url: string) => {
    modal.setContent(content);
    setupModalForm(url);
  };

  const show = (element: Element) => {
    const url = element.getAttribute(RoleActionAttribute)!;

    $.get(url, (response) => {
      setupModal(response, url);
      modal.show();
    });
  };

  addRolesButton.addEventListener('click', () => {
    show(addRolesButton);
  });

  results.addEventListener('click', (event) => {
    const target = event.target as HTMLInputElement;
    const element = target.closest(`[${RoleActionAttribute}]`);

    if (!element) {
      return;
    }

    show(element);
  });

  searchRoles.search();
};
