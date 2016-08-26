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

    let ellipses = '…';
    if (text.length <= length || text.length - ellipses.length <= length) {
      return text;
    }
    else {
      let shortString = text.substr(0, length);
      let splitIndex = shortString.lastIndexOf(' ');
      if (splitIndex <= 0) {
        splitIndex = length;
      }
      return shortString.substr(0, Math.min(length, splitIndex))+ellipses;
    }
  }
}