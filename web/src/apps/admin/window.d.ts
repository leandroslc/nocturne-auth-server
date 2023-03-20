type ManageApplicationOptions = {
  elementsIds: {
    clientType: string;
    consentType: string;
    allowedScopes: string;
    allowClientCredentialsFlow: string;
    allowPasswordFlow: string;
    allowAuthorizationCodeFlow: string;
    allowImplicitFlow: string;
    allowRefreshTokenFlow: string;
    allowLogoutEndpoint: string;
    postLogoutRedirectUris: string;
    redirectUris: string;
  };
  allowedScopes: string[];
  constants: {
    confidentialClientType: string;
  };
};

type ViewApplicationOptions = {
  ids: {
    clientId: string;
    clientSecret: string;
    copyClientId: string;
    copyClientSecret: string;
    toggleClientSecret: string;
  };
};

type SearchPermissionsOptions = {
  searchForm: HTMLFormElement;
  searchUrl: string;
  results: HTMLElement;
};

type SearchPermissionsReturn = {
  search: () => void;
};

type ManagePermissionsOptions = SearchPermissionsOptions & {
  modalId: string;
  newPermissionButton: HTMLElement;
};

type SearchRolePermissionsOptions = {
  searchForm: HTMLFormElement;
  searchUrl: string;
  results: HTMLElement;
};

type SearchRolePermissionsReturn = {
  search: () => void;
};

type ManageRolePermissionsOptions = SearchRolePermissionsOptions & {
  modalId: string;
  addPermissionsButton: HTMLElement;
};

type SearchRolesOptions = {
  searchForm: HTMLFormElement;
  searchUrl: string;
  results: HTMLElement;
};

type SearchRolesReturn = {
  search: () => void;
};

type ManageRolesOptions = {
  ids: {
    modalId: string;
    deleteRoleButton: string;
  };
  deleteRoleAction: string;
  deleteRoleReturnUrl: string;
};

type SearchUserRolesOptions = {
  searchForm: HTMLFormElement;
  searchUrl: string;
  results: HTMLElement;
};

type SearchUserRolesReturn = {
  search: () => void;
};

type ManageUserRolesOptions = SearchUserRolesOptions & {
  modalId: string;
  addRolesButton: HTMLElement;
};

interface Window {
  manageApplication: (options: ManageApplicationOptions) => void;
  viewApplication: (options: ViewApplicationOptions) => void;
  searchPermissions: (
    options: SearchPermissionsOptions,
  ) => SearchPermissionsReturn;
  managePermissions: (options: ManagePermissionsOptions) => void;
  searchRolePermissions: (
    options: SearchRolePermissionsOptions,
  ) => SearchRolePermissionsReturn;
  manageRolePermissions: (options: ManageRolePermissionsOptions) => void;
  searchRoles: (options: SearchRolesOptions) => SearchRolesReturn;
  manageRoles: (options: ManageRolesOptions) => void;
  searchUserRoles: (options: SearchUserRolesOptions) => SearchUserRolesReturn;
  manageUserRoles: (options: ManageUserRolesOptions) => void;
}
