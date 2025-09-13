<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'

const route = useRoute()
const product = ref(null)
const relatedProducts = ref([])
const loading = ref(true)

const fetchProduct = async () => {
  try {
    const response = await fetch(`/api/products/${route.params.id}`)
    product.value = await response.json()
    loading.value = false
    fetchRelatedProducts()
  } catch (error) {
    console.error('Error fetching product:', error)
    loading.value = false
  }
}

const fetchRelatedProducts = async () => {
  try {
    const response = await fetch(`/api/products/${route.params.id}/related`)
    relatedProducts.value = await response.json()
  } catch (error) {
    console.error('Error fetching related products:', error)
  }
}

onMounted(() => {
  fetchProduct()
})
</script>

<template>
  <div class="product-details">
    <div v-if="loading" class="loading">Loading...</div>
    
    <div v-else-if="product" class="product-container">
      <div class="product-header">
        <img :src="product.imageUrl" :alt="product.name" class="product-image" />
        <div class="product-info">
          <h1>{{ product.name }}</h1>
          <p class="price">${{ product.price }}</p>
          <p class="stock-status" :class="{ 'in-stock': product.isInStock }">
            {{ product.isInStock ? 'In Stock' : 'Out of Stock' }}
          </p>
          <button 
            class="add-to-cart"
            :disabled="!product.isInStock"
          >
            Add to Cart
          </button>
        </div>
      </div>

      <div class="expandable-sections">
        <details open>
          <summary>Description</summary>
          <p>Detailed product description goes here.</p>
        </details>

        <details>
          <summary>Specifications</summary>
          <ul>
            <li>Category: {{ product.category }}</li>
            <li>ID: {{ product.id }}</li>
          </ul>
        </details>
      </div>

      <div v-if="relatedProducts.length" class="related-products">
        <h2>Related Products</h2>
        <div class="carousel">
          <div class="carousel-items">
            <div 
              v-for="relatedProduct in relatedProducts" 
              :key="relatedProduct.id"
              class="carousel-item"
            >
              <img :src="relatedProduct.imageUrl" :alt="relatedProduct.name" />
              <h3>{{ relatedProduct.name }}</h3>
              <p>${{ relatedProduct.price }}</p>
              <router-link 
                :to="{ name: 'product-details', params: { id: relatedProduct.id }}"
                class="view-details"
              >
                View Details
              </router-link>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div v-else class="error">
      Product not found
    </div>
  </div>
</template>

<style scoped>
.product-details {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
}

.product-container {
  display: flex;
  flex-direction: column;
  gap: 30px;
}

.product-header {
  display: flex;
  gap: 30px;
}

.product-image {
  width: 400px;
  height: 400px;
  object-fit: cover;
  border-radius: 8px;
}

.product-info {
  flex: 1;
}

.price {
  font-size: 2em;
  font-weight: bold;
  color: #2c3e50;
}

.stock-status {
  color: #e74c3c;
  font-size: 1.1em;
  margin: 10px 0;
}

.stock-status.in-stock {
  color: #27ae60;
}

.add-to-cart {
  padding: 12px 24px;
  background-color: #3498db;
  color: white;
  border: none;
  border-radius: 4px;
  font-size: 1.1em;
  cursor: pointer;
}

.add-to-cart:disabled {
  background-color: #bdc3c7;
  cursor: not-allowed;
}

.expandable-sections {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

details {
  border: 1px solid #ddd;
  border-radius: 4px;
  padding: 10px;
}

summary {
  cursor: pointer;
  font-weight: bold;
  padding: 10px;
}

.related-products {
  margin-top: 30px;
}

.carousel {
  overflow: hidden;
}

.carousel-items {
  display: flex;
  gap: 20px;
  overflow-x: auto;
  padding: 20px 0;
}

.carousel-item {
  min-width: 200px;
  text-align: center;
}

.carousel-item img {
  width: 100%;
  height: 150px;
  object-fit: cover;
  border-radius: 4px;
}

.view-details {
  display: inline-block;
  padding: 8px 16px;
  background-color: #3498db;
  color: white;
  text-decoration: none;
  border-radius: 4px;
  margin-top: 10px;
}

@media (max-width: 768px) {
  .product-header {
    flex-direction: column;
  }

  .product-image {
    width: 100%;
    height: 300px;
  }
}
</style>