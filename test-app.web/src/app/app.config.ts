import { InjectionToken } from '@angular/core';
import { IAppConfig } from './_interfaces/IAppConfig';

export let APP_CONFIG = new InjectionToken('app.config');

export const AppConfig: IAppConfig = {
  // Develop
  endPoint: 'https://localhost:44362',
  apiEndPoint: `https://localhost:44362/api/`,

  // Production
  // endPoint: 'http://46.101.178.188:5001',
  // apiEndPoint: 'http://46.101.178.188:5001/api/',
  baseTitle: 'Test app',
};
