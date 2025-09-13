<script setup>
import { ref, onMounted } from 'vue'

const products = ref([])
const loading = ref(true)
const search = ref('')
const selectedCategory = ref('')
const minPrice = ref(0)
const maxPrice = ref(1000)
const currentPage = ref(1)
const pageSize = ref(10)
const categories = ref([])

async function fetchProducts() {
  try {
  const response = await fetch(`/api/products?search=${search.value}&category=${selectedCategory.value}&minPrice=${minPrice.value}&maxPrice=${maxPrice.value}&page=${currentPage.value}&pageSize=${pageSize.value}`)
    const data = await response.json()
    products.value = data.items
    loading.value = false
  } catch (error) {
    console.error('Error fetching products:', error)
    loading.value = false
  }
}

onMounted(async () => {
  await fetchProducts()
  // Get unique categories from products
  categories.value = [...new Set(products.value.map(p => p.category))]
})

const handleSearch = () => {
  currentPage.value = 1
  fetchProducts()
}

const handlePageChange = (page) => {
  currentPage.value = page
  fetchProducts()
}

const handleFiltersChange = () => {
  currentPage.value = 1
  fetchProducts()
}
</script>

<template>
  <div class="product-list">
    <div class="filters">
      <input 
        v-model="search"
        type="text"
        placeholder="Search products..."
        @input="handleSearch"
        class="search-input"
      />
      
      <select v-model="selectedCategory" @change="handleFiltersChange" class="category-dropdown">
        <option value="">All Categories</option>
        <option v-for="category in categories" :key="category" :value="category">
          {{ category }}
        </option>
      </select>

      <div class="price-range">
        <input 
          type="range"
          v-model="minPrice"
          :min="0"
          :max="1000"
          @change="handleFiltersChange"
        />
        <input 
          type="range"
          v-model="maxPrice"
          :min="0"
          :max="1000"
          @change="handleFiltersChange"
        />
        <div>
          Price: ${{ minPrice }} - ${{ maxPrice }}
        </div>
      </div>
    </div>

    <div v-if="loading" class="loading">Loading...</div>

    <div v-else class="products-grid">
      <div v-for="product in products" :key="product.id" class="product-card">
        <router-link :to="{ name: 'product-details', params: { id: product.id }}" class="product-link">
          <img :src="product.imageUrl" :alt="product.name" class="product-image" />
          <div class="product-info">
            <h3>{{ product.name }}</h3>
            <p class="price">${{ product.price }}</p>
            <p class="stock-status" :class="{ 'in-stock': product.isInStock }">
              {{ product.isInStock ? 'In Stock' : 'Out of Stock' }}
            </p>
            <button 
              class="add-to-cart"
              :disabled="!product.isInStock"
              @click.prevent
            >
              Add to Cart
            </button>
          </div>
        </router-link>
      </div>
    </div>

    <div class="pagination">
      <button 
        :disabled="currentPage === 1"
        @click="handlePageChange(currentPage - 1)"
      >
        Previous
      </button>
      <span>Page {{ currentPage }}</span>
      <button 
        @click="handlePageChange(currentPage + 1)"
      >
        Next
      </button>
      <select v-model="pageSize" @change="handleFiltersChange">
        <option :value="5">5 per page</option>
        <option :value="10">10 per page</option>
        <option :value="20">20 per page</option>
      </select>
    </div>
  </div>
</template>

<style scoped>
.product-list {
  padding: 20px;
}

.filters {
  margin-bottom: 20px;
  display: flex;
  gap: 20px;
  align-items: center;
}

.search-input {
  padding: 8px;
  border: 1px solid #ddd;
  border-radius: 4px;
  width: 200px;
}

.category-dropdown {
  padding: 8px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.price-range {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.products-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 20px;
}

.product-card {
  border: 1px solid #ddd;
  border-radius: 8px;
  overflow: hidden;
  transition: transform 0.2s;
}

.product-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.product-link {
  text-decoration: none;
  color: inherit;
  display: block;
}

.product-image {
  width: 100%;
  height: 200px;
  object-fit: cover;
}

.product-info {
  padding: 15px;
}

.price {
  font-size: 1.2em;
  font-weight: bold;
  color: #2c3e50;
}

.stock-status {
  color: #e74c3c;
  margin: 5px 0;
}

.stock-status.in-stock {
  color: #27ae60;
}

.add-to-cart {
  width: 100%;
  padding: 8px;
  background-color: #3498db;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.add-to-cart:disabled {
  background-color: #bdc3c7;
  cursor: not-allowed;
}

.pagination {
  margin-top: 20px;
  display: flex;
  gap: 10px;
  align-items: center;
  justify-content: center;
}

.pagination button {
  padding: 8px 16px;
  border: 1px solid #ddd;
  background-color: white;
  cursor: pointer;
}

.pagination button:disabled {
  background-color: #f5f5f5;
  cursor: not-allowed;
}

.loading {
  text-align: center;
  padding: 20px;
  font-size: 1.2em;
}
</style>