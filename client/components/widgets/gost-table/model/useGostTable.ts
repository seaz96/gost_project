import type { DataTableColumns } from 'naive-ui'
import type { Gost } from '~/components/shared/types/gost'

export const useGostTable = () => {
  const createColumns = (): DataTableColumns<Gost> => {
    return [
      {
        key: 'no',
        className: 'table-def-2__col',
        title: () => h('div', { class: 'table-def-2__text' }, { default: () => '№' }),
        render: row => h('div', { class: 'table-def-2__text' }, { default: () => row.docId })
      },
      {
        key: 'title',
        className: 'table-def-2__col',
        title: () => h('div', { class: 'table-def-2__text' }, { default: () => "Обозначение" }),
        render: row => h(
          'div',
          { class: 'table-def-2__text' },
          { default: () => row.primary.designation })
      },
      {
        key: 'title',
        className: 'table-def-2__col',
        title: () => h('div', { class: 'table-def-2__text' }, { default: () => "Код ОКС" }),
        render: row => h(
          'div',
          { class: 'table-def-2__text' },
          { default: () => row.primary.codeOKS })
      },
      {
        key: 'title',
        className: 'table-def-2__col',
        title: () => h('div', { class: 'table-def-2__text' }, { default: () => "Наименование" }),
        render: row => h(
          'div',
          { class: 'table-def-2__text' },
          { default: () => row.primary.fullName })
      },
      // {
      //   title: () => h('div', { class: 'table-def-2__text' }, { default: () => '' }),
      //   render: row => renderActions(row),
      //   key: 'actions',
      //   className: 'table-def-2__col'
      // }
    ]
  }

  return {
    createColumns
  }
}
