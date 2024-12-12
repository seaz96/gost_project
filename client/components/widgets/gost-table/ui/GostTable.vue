<template>
  <n-data-table
    :bordered="false"
    :single-line="false"
    :columns="columns"
    :data="props.gosts"
  />
</template>

<script setup lang="ts">
import type { Gost } from '~/components/shared/types/gost';
import { NDataTable } from 'naive-ui';
import { useGostTable } from '../model/useGostTable';

const props = defineProps<{
  gosts: Gost[]
}>()

const { createColumns } = useGostTable()
const columns = ref(createColumns())
</script>