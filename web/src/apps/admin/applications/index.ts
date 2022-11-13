window.manageApplication = (options) => {
  const { constants } = options;
  const { elementsIds } = options;
  const { allowedScopes } = options;

  const elements = {
    redirectUris: window.ElementHelper.getRequiredElementById(
      elementsIds.redirectUris,
    ) as HTMLInputElement,
    postLogoutRedirectUris: window.ElementHelper.getRequiredElementById(
      elementsIds.postLogoutRedirectUris,
    ) as HTMLInputElement,
    allowedScopes: window.ElementHelper.getRequiredElementById(
      elementsIds.allowedScopes,
    ) as HTMLInputElement,
    clientType: window.ElementHelper.getRequiredElementById(
      elementsIds.clientType,
    ) as HTMLInputElement,
    allowClientCredentialsFlow: window.ElementHelper.getRequiredElementById(
      elementsIds.allowClientCredentialsFlow,
    ) as HTMLInputElement,
    allowPasswordFlow: window.ElementHelper.getRequiredElementById(
      elementsIds.allowPasswordFlow,
    ) as HTMLInputElement,
    allowAuthorizationCodeFlow: window.ElementHelper.getRequiredElementById(
      elementsIds.allowAuthorizationCodeFlow,
    ) as HTMLInputElement,
    allowImplicitFlow: window.ElementHelper.getRequiredElementById(
      elementsIds.allowImplicitFlow,
    ) as HTMLInputElement,
    allowRefreshTokenFlow: window.ElementHelper.getRequiredElementById(
      elementsIds.allowRefreshTokenFlow,
    ) as HTMLInputElement,
    allowLogoutEndpoint: window.ElementHelper.getRequiredElementById(
      elementsIds.allowLogoutEndpoint,
    ) as HTMLInputElement,
    consentType: window.ElementHelper.getRequiredElementById(
      elementsIds.consentType,
    ) as HTMLSelectElement,
  };

  const redirectUriTagify = new window.TagifyElement(elements.redirectUris);
  const postLogoutRedirectUrisTagify = new window.TagifyElement(
    elements.postLogoutRedirectUris,
  );

  // eslint-disable-next-line no-new
  new window.TagifyElement(elements.allowedScopes, {
    allowedEntries: allowedScopes,
  });

  const setDisabled = (element: HTMLElement, isDisabled: boolean) => {
    if (isDisabled) {
      element.setAttribute('disabled', 'disabled');
    } else {
      element.removeAttribute('disabled');
    }
  };

  const checkClientType = () => {
    const clientTypeElement = elements.clientType;
    const allowClientCredentialsElement = elements.allowClientCredentialsFlow;
    const clientType = clientTypeElement.value;

    if (clientType === constants.confidentialClientType) {
      setDisabled(allowClientCredentialsElement, false);
    } else {
      setDisabled(allowClientCredentialsElement, true);
      allowClientCredentialsElement.checked = false;
    }
  };

  const checkRefreshTokenFlow = () => {
    const isPasswordFlow = elements.allowPasswordFlow.checked;
    const isAuthorizationCode = elements.allowAuthorizationCodeFlow.checked;

    const { allowRefreshTokenFlow } = elements;

    if (isAuthorizationCode || isPasswordFlow) {
      setDisabled(allowRefreshTokenFlow, false);
    } else {
      setDisabled(allowRefreshTokenFlow, true);
      allowRefreshTokenFlow.checked = false;
    }
  };

  const checkLogoutEndpoint = () => {
    if (elements.allowLogoutEndpoint.checked) {
      postLogoutRedirectUrisTagify.enable();
    } else {
      postLogoutRedirectUrisTagify.disableAndClean();
    }
  };

  const checkSettings = () => {
    const { allowLogoutEndpoint } = elements;
    const { consentType } = elements;

    const isImplicitFlow = elements.allowImplicitFlow.checked;
    const isAuthorizationCode = elements.allowAuthorizationCodeFlow.checked;

    if (isAuthorizationCode || isImplicitFlow) {
      setDisabled(allowLogoutEndpoint, false);
      setDisabled(consentType, false);
      redirectUriTagify.enable();

      checkLogoutEndpoint();
    } else {
      setDisabled(allowLogoutEndpoint, true);
      setDisabled(consentType, true);
      redirectUriTagify.disableAndClean();
      postLogoutRedirectUrisTagify.disableAndClean();

      allowLogoutEndpoint.checked = false;
      consentType.selectedIndex = 0;
    }
  };

  const checkFlows = () => {
    checkRefreshTokenFlow();
    checkSettings();
  };

  elements.clientType.addEventListener('change', () => {
    checkClientType();
  });

  const addFlowChangeEvent = (element: HTMLElement) => {
    element.addEventListener('change', () => {
      checkFlows();
    });
  };

  addFlowChangeEvent(elements.allowAuthorizationCodeFlow);
  addFlowChangeEvent(elements.allowClientCredentialsFlow);
  addFlowChangeEvent(elements.allowImplicitFlow);
  addFlowChangeEvent(elements.allowPasswordFlow);
  addFlowChangeEvent(elements.allowRefreshTokenFlow);

  elements.allowLogoutEndpoint.addEventListener('change', () => {
    checkLogoutEndpoint();
  });

  checkClientType();
  checkFlows();
};

window.viewApplication = (options) => {
  const clientIdElement = window.ElementHelper.getRequiredElementById(
    options.ids.clientId,
  ) as HTMLInputElement;
  const clientSecretElement = window.ElementHelper.getRequiredElementById(
    options.ids.clientSecret,
  ) as HTMLInputElement;
  const copyClientIdElement = window.ElementHelper.getRequiredElementById(
    options.ids.copyClientId,
  );
  const copyClientSecretElement = window.ElementHelper.getRequiredElementById(
    options.ids.copyClientSecret,
  );
  const toggleClientSecretElement = window.ElementHelper.getRequiredElementById(
    options.ids.toggleClientSecret,
  );

  function addCopyEvent(element: HTMLInputElement, button: HTMLElement) {
    button.addEventListener('click', () => {
      window.ElementHelper.copyText(element);
    });
  }

  addCopyEvent(clientIdElement, copyClientIdElement);

  if (clientSecretElement) {
    addCopyEvent(clientSecretElement, copyClientSecretElement);
    window.ElementHelper.addTogglePasswordEvent(
      clientSecretElement,
      toggleClientSecretElement,
    );
  }
};
