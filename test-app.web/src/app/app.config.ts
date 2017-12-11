import { InjectionToken } from '@angular/core';
import { IAppConfig } from './_interfaces/IAppConfig';

export let APP_CONFIG = new InjectionToken('app.config');

export const AppConfig: IAppConfig = {
  // Develop
  endPoint: 'https://localhost:44362',
  apiEndPoint: `https://localhost:44362/api/`,

  // Production
  // endPoint: 'https://localhost:5000',
  // apiEndPoint: 'http://localhost:5000/api/',
  baseTitle: 'Test app',
};
