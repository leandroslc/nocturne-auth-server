type ScopeSelectionOptions = {
  scopesContainerId: string;
};

interface Window {
  scopeSelect: (options: ScopeSelectionOptions) => void;
}
