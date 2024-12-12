<template>
  <div>
    <section class="search-section">
      <Search />
    </section>
    <section>
      <GostTable :gosts="gosts"/>
    </section>
  </div>
</template>

<script setup lang="ts">
import { GostApi } from '../features/gost/api/GostApi';
import isAuthenticated from '../middlewares/isAuthenticated';
import type { Gost } from '../shared/types/gost';
import GostTable from '../widgets/gost-table/ui/GostTable.vue';
import Search from '../widgets/search/ui/Search.vue';

const gosts = ref<Gost[]>([])
const gostApi = GostApi()

const loadGosts = async () => {
  gosts.value = await gostApi.loadGosts(0)
}

await useAsyncData('loadGosts', async () => {
  loadGosts()
})

definePageMeta({
  middleware: [isAuthenticated]
})
</script>