<template>
  <div class="main-search">
    <input
      v-model="searchText"
      type="text"
      placeholder="Поиск"
      class="search-input"
    >
    <IconButton
      img="/img/filter.png"
    />
    <IconButton
      img="/img/search.png"
      class="search-button"
    />
  </div>
  
</template>

<script setup lang="ts">
import IconButton from '~/components/shared/ui/buttons/IconButton.vue';

const searchText = ref('')
</script>

<style lang="scss">
.main-search {
  display: flex;
  margin-top: 25px;
  height: 50px;

  .search-input {
    font-family: "Manrope", Arial, Helvetica, sans-serif;
    font-size: 15px;
    font-weight: 400;
    line-height: 20px;
    width: 100%;
    border: 1px solid #D2D2D2;
    padding: 14px 20px 14px 20px;
    border-radius: 5px 0px 0px 5px;
  }

  .search-button {
    border-radius: 0px 5px 5px 0px;
  }
}


</style>