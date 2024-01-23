import React, {useState} from 'react'
import { Button, Input } from 'shared/components'

import styles from './ReviewsStatisticForm.module.scss'
import axios from 'axios'

interface ReviewsStatisticFormProps {
  handleSubmit: Function,
  startDateSubmit: Function,
  endDateSubmit: Function
}

const ReviewsStatisticForm:React.FC<ReviewsStatisticFormProps> = props => {
  const {
    handleSubmit,
    startDateSubmit,
    endDateSubmit
  } = props

  const [reviewsData, setReviewsData] = useState({
    gostName: '',
    codeOKS: '',
    activityField: '',
    startDate: '',  
    endDate: ''
  })
  

  const validateData = (event: React.FormEvent) => {
    event.preventDefault()
    axios.get('https://backend-seaz96.kexogg.ru/api/stats/get-views', {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('jwt_token')}`
      },
      params: {
        ...reviewsData,
        StartDate: new Date(reviewsData.startDate).toISOString(),
        EndDate: new Date(reviewsData.endDate).toISOString()
      }
    }).then(response => {
      handleSubmit(response.data)
      startDateSubmit(reviewsData.startDate)
      endDateSubmit(reviewsData.endDate)
    })
  }

  return (
    <form className={styles.form} onSubmit={(event) => validateData(event)}>
        <Input 
          label='Название ГОСТа' 
          type='text' 
          value={reviewsData.gostName}
          onChange={(value: string) => setReviewsData({...reviewsData, gostName: value})}
        />
        <Input 
          label='Код ОКС' 
          type='text' 
          value={reviewsData.codeOKS}
          onChange={(value: string) => setReviewsData({...reviewsData, codeOKS: value})}
        />
        <Input 
          label='Сфера деятельности' 
          type='text' 
          value={reviewsData.activityField}
          onChange={(value: string) => setReviewsData({...reviewsData, activityField: value})}
        />
        <p className={styles.datesTitle}>Даты обращение к карте(от, до)</p>
        <Input 
          placeholder='От...' 
          type='date' 
          value={reviewsData.startDate}
          onChange={(value: string) => setReviewsData({...reviewsData, startDate: value})}
        />
        <Input 
          placeholder='До...' 
          type='date' 
          value={reviewsData.endDate}
          onChange={(value: string) => setReviewsData({...reviewsData, endDate: value})}
        />
        <Button onClick={() => {}} className={styles.formButton} isFilled type='submit'>Сформировать отчёт</Button>
    </form>
  )
}

export default ReviewsStatisticForm