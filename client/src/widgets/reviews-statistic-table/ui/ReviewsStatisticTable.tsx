import React from 'react'

import styles from './ReviewsStatisticTable.module.scss'
import { gostModel } from 'entities/gost'

interface ReviewsStatisticTableProps {
  reviewsData: gostModel.GostViews[]
}

const ReviewsStatisticTable:React.FC<ReviewsStatisticTableProps> = props => {
  const {
    reviewsData
  } = props

  return (
    <table className={styles.table}>
        <thead>
          <tr className={styles.tableRow}>
            <th>№</th>
            <th>Обозначение</th>
            <th>Сфера деятельности</th>
            <th>Просмотры</th>
          </tr>
        </thead>
        <tbody>
          {
            reviewsData.map(review => 
              <tr>
                <td>{review.docId}</td>
                <td>{review.designation}</td>
                <td>{review.fullName}</td>
                <td>{review.views}</td>
              </tr>
            )
          }
        </tbody>
    </table>
  )
}

export default ReviewsStatisticTable;