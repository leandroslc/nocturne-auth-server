(function () {
  window.SearchRoles = function(options) {
    const form = options.searchForm;
    const searchUrl = options.searchUrl;
    const results = options.results;

    const self = this;
    const formLoading = Loading.create(form);

    form.addEventListener('submit', function (event) {
      event.preventDefault();

      self.search();
    });

    this.search = function() {
      formLoading.start();

      const data = $(form).serialize();

      $.get(searchUrl, data, function (response) {
        results.innerHTML = response;
        formLoading.stop();
      });
    }
  };
})();

(function () {
  const RoleActionAttribute = 'data-role-action';
  const ApplicationChangeAttribute = 'data-application-change';

  window.manageRoles = function(options) {
    const searchRoles = new SearchRoles(options);
    const results = options.results;
    const modalId = options.modalId;
    const addRolesButton = options.addRolesButton;

    const modal = new Modal(modalId);

    function createModalFormSubmitEvent(form, url) {
      const formLoading = Loading.create(form);

      return function(event) {
        event.preventDefault();

        if (!Validator.isValid(form)) {
          return;
        }

        formLoading.start();

        const data = $(form).serialize();

        $.post(url, data, function () {
          searchRoles.search();
          modal.close();
        }).fail(function (response) {
          if (ResponseHelper.isBadRequest(response) && ResponseHelper.isHtml(response)) {
            setupModal(response.responseText, url);
          }

          modal.close();
        });
      };
    }

    function createChangeApplicationEvent(form, baseApplicationChangeUrl) {
      const formLoading = Loading.create(form);

      return function (event) {
        event.preventDefault();

        const applicationId = event.target.value;
        const url = baseApplicationChangeUrl + '?applicationId=' + applicationId;

        formLoading.start();

        $.get(url, function (response) {
          setupModal(response, url);
        });
      }
    }

    function setupModalForm(url) {
      const form = modal.querySelector('form');

      if (!form) {
        return;
      }

      Validator.attach(form);

      form.addEventListener('submit', createModalFormSubmitEvent(form, url));

      const changeApplicationElement = form.querySelector('[' + ApplicationChangeAttribute  + ']');

      if (changeApplicationElement) {
        const baseApplicationChangeUrl = changeApplicationElement.getAttribute(ApplicationChangeAttribute);

        changeApplicationElement.addEventListener(
          'change', createChangeApplicationEvent(form, baseApplicationChangeUrl));
      }
    }

    function setupModal(content, url) {
      modal.setContent(content);
      setupModalForm(url);
    }

    function show(element) {
      const url = element.getAttribute(RoleActionAttribute);

      $.get(url, function (response) {
        setupModal(response, url);
        modal.show();
      });
    }

    addRolesButton.addEventListener('click', function() {
      show(addRolesButton);
    });

    results.addEventListener('click', function (event) {
      const element = event.target.closest('[' + RoleActionAttribute + ']');

      if (!element) {
        return;
      }

      show(element);
    });

    searchRoles.search();
  }
})();
