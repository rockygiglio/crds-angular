import { ReplaceNonAlphaNumericPipe } from '../../../app/media/pipes/replace-non-alpha-numeric.pipe';

describe('TruncatePipe', () => {
  let pipe: ReplaceNonAlphaNumericPipe;

  beforeEach(() => {
    pipe = new ReplaceNonAlphaNumericPipe();
  });

  it('Replace nonAlphaNumeric Characters in a string', () => {
    let text = 'Death to Separation. Long Live Unity';

    let result = 'Death-to-Separation--Long-Live-Unity';
    expect(pipe.transform(text)).toEqual(result);
  });

});
