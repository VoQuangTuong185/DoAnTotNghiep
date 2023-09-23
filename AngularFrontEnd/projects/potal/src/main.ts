import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from '../../features/src/app/app.module';


platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
