<template>
    <button class="iconButton">
        <img :src="img" >
    </button>
</template>

<script setup lang="ts">
defineProps<{img: string}>()
</script>

<style lang="scss">
.iconButton {
    padding: 8px;
    border: none;
    background: linear-gradient(180deg, #CB0E70 -26%, #F1B72C 183%);
}
</style>