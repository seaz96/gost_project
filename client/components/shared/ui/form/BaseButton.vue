<template>
  <button 
    @click.prevent="action"
    class="baseButton"
    :class="['baseButton', isColored && 'baseButton_colored']"
  >
    <img v-if="prefixIcon" :src="prefixIcon" alt="" class="baseButton__icon baseButton__icon_prefix">
    <p>{{ text }}</p>
    <img v-if="suffixIcon" :src="suffixIcon" alt="" class="baseButton__icon baseButton__icon_suffix">
  </button>
</template>

<script setup lang="ts">
export type ButtonProps = {
  action?: () => void,
  text: string,
  isColored?: boolean,
  prefixIcon?: string,
  suffixIcon?: string
}

withDefaults(defineProps<ButtonProps>(), {
  action: () => {},
  text: '',
  isColored: true,
  prefixIcon: '',
  suffixIcon: ''
})
</script>

<style lang="scss">
.baseButton {
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 13px 10px 13px 10px;
  font-family: "Manrope", Arial, Helvetica, sans-serif;
  font-size: 15px;
  line-height: 20px;
  font-weight: 700;
  background-color: inherit;
  border: none;
  outline: none;
  color: rgba(209, 41, 101, 1);

  &_colored {
    background: linear-gradient(180deg, #CB0E70 -26%, #F1B72C 183%);
    border-radius: 5px;
    color: #ffffff;
  }

  &__icon {
    width: 16px;
    height: 16px;

    &_prefix {
      margin-right: 8px;
    }

    &_suffix {
      margin-left: 8px;
    }
  }
}

.baseButton:before {
  content: "";
  position: absolute;
  z-index: -1;
  inset: 0;
  padding: 1px; /* the border thickness */
  border-radius: 5px;
  background: linear-gradient(180deg, #CB0E70 -26%, #F1B72C 183%);
  
  mask: 
   linear-gradient(#000 0 0) exclude, 
   linear-gradient(#000 0 0) content-box;

}
</style>