import ProductSummaryController from './product_summary.controller';
import ProductSummaryTemplate from './product_summary.html';
import { getCampProductInfo } from '../../camps.resolves';

const ProductSummary = {
  bindings: {},
  template: ProductSummaryTemplate,
  controller: ProductSummaryController,
  resolve: [getCampProductInfo]
};

export default ProductSummary;
