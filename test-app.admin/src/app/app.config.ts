import { InjectionToken } from '@angular/core';
import { IAppConfig } from './_interfaces/IAppConfig';

export let APP_CONFIG = new InjectionToken('app.config');

export const AppConfig: IAppConfig = {
  baseTitle: 'Test app',
};
