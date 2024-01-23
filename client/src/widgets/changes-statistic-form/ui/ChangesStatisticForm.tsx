import React, {useState} from 'react'
import { Button, Input, RadioGroup } from 'shared/components'

import styles from './ChangesStatisticForm.module.scss'
import axios from 'axios'

interface ChangesStatisticFormProps {
  handleSubmit: Function,
  startDateSubmit: Function,
  endDateSubmit: Function
}

const ChangesStatisticForm:React.FC<ChangesStatisticFormProps> = props => {
  const {
    handleSubmit,
    startDateSubmit,
    endDateSubmit
  } = props

  const [changesData, setChangesData] = useState({
    status: 0 | 1 | 2,
    dateFrom: '',  
    dateTo: ''
  })

  const validateData = (event: React.FormEvent) => {
    event.preventDefault()
    axios.get('https://backend-seaz96.kexogg.ru/api/stats/get-count', {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('jwt_token')}`
      },
      params: {
        ...changesData,
        StartDate: new Date(changesData.dateFrom).toISOString(),
        EndDate: new Date(changesData.dateTo).toISOString()
      }
    }).then(response => {
      console.log(response.data)
      handleSubmit(response.data)
      startDateSubmit(changesData.dateFrom)
      endDateSubmit(changesData.dateTo)
    })
    
    handleSubmit()
  }

  return (
    <form className={styles.form} onSubmit={(event) => validateData(event)}>
        <p className={styles.status}>Статус</p>
        <RadioGroup 
          buttons={[
            {id:'Cancelled', value:'0', label:'Отменем'},
            {id:'Replaced', value:'1', label:'Замененный'},
            {id:'Current', value:'2', label:'Действующий'},
          ]} 
          name='status'
          value={changesData.status.toString()}
          onChange={(value: string) => {setChangesData({...changesData, status: parseInt(value)})}}
        />
        <p className={styles.datesTitle}>Даты обращение к карте(от, до)</p>
        <Input 
          placeholder='От...' 
          type='date' 
          value={changesData.dateFrom}
          onChange={(value: string) => setChangesData({...changesData, dateFrom: value})}
        />
        <Input 
          placeholder='До...' 
          type='date' 
          value={changesData.dateTo}
          onChange={(value: string) => setChangesData({...changesData, dateTo: value})}
        />
        <Button onClick={() => {}} className={styles.formButton} isFilled type='submit'>Сформировать отчёт</Button>
    </form>
  )
}

export default ChangesStatisticForm