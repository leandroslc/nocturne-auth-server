import { Providers } from './types';

let globalProviders: Providers;

export const setProviders = (providers: Providers) => {
  globalProviders = providers;
};

export const getProvider = <T extends keyof Providers>(
  name: T,
): Providers[T] => {
  return globalProviders[name];
};
