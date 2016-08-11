import { TruncatePipe } from '../../../core/pipes/truncate.pipe';

describe('TruncatePipe', () => {
  let pipe: TruncatePipe;

  beforeEach(() => {
    pipe = new TruncatePipe();
  });

  it('Truncate text to 10 characters', () => {
    let text = 'Fusce dapibus, tellus ac cursus commodo, tortor mauris condimentum nibh, ut fermentum massa justo sit amet risus. Vestibulum id ligula porta felis euismod semper. Curabitur blandit tempus porttitor.';
    expect(pipe.transform(text, 10)).toEqual('Fusce…');
  });

});
