import React from 'react'

import styles from './ChangesStatisticTable.module.scss'
import { gostModel } from 'entities/gost'

interface ChangesStatisticTableProps {
    changesData: gostModel.GostChanges
}

enum actions {
  'Update'='Изменение',
  'Create'='Создание'
}

const ChangesStatisticTable:React.FC<ChangesStatisticTableProps> = props => {
  const {
    changesData
  } = props

  return (
    <table className={styles.table}>
        <thead>
          <tr className={styles.tableRow}>
            <th>№</th>
            <th>Обозначение</th>
            <th>Наименование</th>
            <th>Действие</th>
            <th>Дата</th>
          </tr>
        </thead>
        <tbody>
          {
            changesData.stats.map(changes => 
              <tr>
                <td>{changes.docId}</td>
                <td>{changes.designation}</td>
                <td>{changes.fullName}</td>
                <td>{actions[changes.action]}</td>
                <td>{formatDate(new Date(changes.date))}</td>
              </tr>
            )
          }
        </tbody>
    </table>
  )
}

const formatDate = (date: Date) => {
    let day = date.getDate().toString();
    day = day.length === 1 ? '0' + day : day;
    const month = date.getMonth() + 1;
    const year = date.getFullYear();
    return `${day}.${month}.${year}`;
}

export default ChangesStatisticTable;