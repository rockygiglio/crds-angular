import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'truncate'
})

export class TruncatePipe implements PipeTransform {
  transform(text: string, length: number) {

    if (!text) {
      return text;
    }

    if (isNaN(length)) {
      length = 10;
    }

    var ellipses = '…';
    if (text.length <= length || text.length - ellipses.length <= length) {
      return text;
    }
    else {
      var shortString = text.substr(0, length);
      return shortString.substr(0, Math.min(length, shortString.lastIndexOf(' ')))+ellipses;
    }
  }
}