import { Pipe, PipeTransform } from '@angular/core';

import _ from 'underscore';

@Pipe({
    name: 'skinsAvailable',
    pure: false
})
export class SkinsAvailablePipe implements PipeTransform {
    transform(items: any[], filter: Object): any {
      var s = _.filter(_.map(items, (el) => {
          return {
            id: el.id,
            marketHashName: el.marketHashName,
            price: el.price
          };
        }), function(obj){ return !_.findWhere(_.map(filter, (el) => {
          return {
            id: el.id,
            marketHashName: el.marketHashName,
            price: el.price
          };
        }), obj); });
      return s;
    }
}
