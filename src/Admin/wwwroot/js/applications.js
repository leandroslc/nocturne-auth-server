
window.manageApplication = function(options) {
  const constants = options.constants;
  const elementsIds = options.elementsIds;
  const allowedScopes = options.allowedScopes;

  const elements = {};

  Object.keys(elementsIds).forEach(function (key) {
    const id = elementsIds[key];
    const element = document.getElementById(id);

    if (!element) {
        throw new Error('Element "' + id + '" not found');
    }

    elements[key] = element;
  });

  const redirectUriTagify = new TagifyElement(elements.redirectUris);
  const postLogoutRedirectUrisTagify = new TagifyElement(elements.postLogoutRedirectUris);

  new TagifyElement(elements.allowedScopes, function(options) {
    options.enforceWhitelist = true;
    options.whitelist = allowedScopes;
  });

  function setDisabled(element, isDisabled) {
    isDisabled
      ? element.setAttribute('disabled', 'disabled')
      : element.removeAttribute('disabled');
  }

  function checkClientType() {
    var clientTypeElement = elements.clientType;
    var allowClientCredentialsElement = elements.allowClientCredentialsFlow;
    var clientType = clientTypeElement.value;

    if (clientType === constants.confidentialClientType) {
      setDisabled(allowClientCredentialsElement, false);
    } else {
      setDisabled(allowClientCredentialsElement, true);
      allowClientCredentialsElement.checked = false;
    }

    clientType === constants.confidentialClientType
  }

  function checkRefreshTokenFlow() {
    var isPasswordFlow = elements.allowPasswordFlow.checked;
    var isAuthorizationCode = elements.allowAuthorizationCodeFlow.checked;

    var allowRefreshTokenFlow = elements.allowRefreshTokenFlow;

    if (isAuthorizationCode || isPasswordFlow) {
      setDisabled(allowRefreshTokenFlow, false);
    }
    else {
      setDisabled(allowRefreshTokenFlow, true);
      allowRefreshTokenFlow.checked = false;
    }
  }

  function checkLogoutEndpoint(isEnabled) {
    if (isEnabled) {
      postLogoutRedirectUrisTagify.enable();
    } else {
      postLogoutRedirectUrisTagify.disableAndClean();
    }
  }

  function checkSettings() {
    var allowLogoutEndpoint = elements.allowLogoutEndpoint;
    var consentType = elements.consentType;

    var isImplicitFlow = elements.allowImplicitFlow.checked;
    var isAuthorizationCode = elements.allowAuthorizationCodeFlow.checked;

    if (isAuthorizationCode || isImplicitFlow) {
      setDisabled(allowLogoutEndpoint, false);
      setDisabled(consentType, false);
      redirectUriTagify.enable();

      checkLogoutEndpoint(allowLogoutEndpoint.checked);
    } else {
      setDisabled(allowLogoutEndpoint, true);
      setDisabled(consentType, true);
      redirectUriTagify.disableAndClean();
      postLogoutRedirectUrisTagify.disableAndClean();

      allowLogoutEndpoint.checked = false;
      consentType.selectedIndex = 0;
    }
  }

  function checkFlows() {
    checkRefreshTokenFlow();
    checkSettings();
  }

  elements.clientType.addEventListener('change', function () {
    checkClientType();
  });

  function addFlowChangeEvent(element) {
    element.addEventListener('change', function () {
        checkFlows();
    });
  }

  addFlowChangeEvent(elements.allowAuthorizationCodeFlow);
  addFlowChangeEvent(elements.allowClientCredentialsFlow);
  addFlowChangeEvent(elements.allowImplicitFlow);
  addFlowChangeEvent(elements.allowPasswordFlow);
  addFlowChangeEvent(elements.allowRefreshTokenFlow);

  elements.allowLogoutEndpoint.addEventListener('change', function () {
      checkLogoutEndpoint(this.checked);
  });

  checkClientType();
  checkFlows();
};

(function () {

  window.viewApplication = function(options) {
    const clientIdElement = ElementHelper.getRequiredElementById(options.ids.clientId);
    const clientSecretElement = document.getElementById(options.ids.clientSecret);
    const copyClientIdElement = ElementHelper.getRequiredElementById(options.ids.copyClientId);
    const copyClientSecretElement = document.getElementById(options.ids.copyClientSecret);
    const toggleClientSecretElement = document.getElementById(options.ids.toggleClientSecret);

    function addCopyEvent(element, button) {
      button.addEventListener('click', function() {
          ElementHelper.copyText(element);
      });
    }

    addCopyEvent(clientIdElement, copyClientIdElement);

    if (clientSecretElement) {
        addCopyEvent(clientSecretElement, copyClientSecretElement);
        ElementHelper.addTogglePasswordEvent(clientSecretElement, toggleClientSecretElement);
    }
  };

})();
