import { createRouter, createWebHistory } from 'vue-router'
import ProductList from '../components/ProductList.vue';
import ProductDetails from '../components/ProductDetails.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'products',
      component: ProductList
    },
    {
      path: '/product/:id',
      name: 'product-details',
      component: ProductDetails
    }
  ]
})

export default router
