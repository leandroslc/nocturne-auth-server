window.manageRoles = (options) => {
  const { ids, deleteRoleAction, deleteRoleReturnUrl } = options;

  const deleteRoleButton = window.ElementHelper.getRequiredElementById(
    ids.deleteRoleButton,
  );

  const modal = new window.Modal(ids.modalId);

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
        modal.close();
        window.location.href = deleteRoleReturnUrl;
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

  const show = () => {
    const url = deleteRoleAction;

    $.get(url, (response) => {
      setupModal(response, url);
      modal.show();
    });
  };

  deleteRoleButton.addEventListener('click', () => {
    show();
  });
};
