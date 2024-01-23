import React, { useState } from 'react'

import styles from './GostForm.module.scss'
import { Button, Input, RadioGroup } from 'shared/components'
import { newGostModel } from '..'
import TextArea from 'shared/components/TextArea'
import { GostToSave } from '../model/newGostModel'

interface GostFormProps {
  handleSubmit: Function
  gost?: GostToSave
}

const GostForm: React.FC<GostFormProps> = props => {
  const {
    handleSubmit,
    gost = {
      "designation": '',
      "fullName": '',
      "codeOKS": '',
      "activityField": '',
      "acceptanceDate": '',
      "commissionDate":  '',
      "author": '',
      "acceptedFirstTimeOrReplaced": '',
      "content": '',
      "keyWords": '',
      "keyPhrases": '',
      "applicationArea": '',
      "adoptionLevel": 0,
      "documentText": '',
      "changes": '',
      "amendments": '',
      "status": 0,
      "harmonization": 1,
      "isPrimary": true,
      "referencesId": 1
    }
  } = props

  const [newGost, setNewGost] = useState<newGostModel.GostToSave>(gost)
  console.log(newGost)
  return (
    <form onSubmit={(event) => {
      event.preventDefault()
      console.log({
        ...newGost,
        acceptanceDate: new Date(newGost.acceptanceDate).toISOString(),
        commissionDate: new Date(newGost.commissionDate).toISOString(),
        referencesId: [0]
      })
      handleSubmit({
        ...newGost,
        acceptanceDate: new Date(newGost.acceptanceDate).toISOString(),
        commissionDate: new Date(newGost.commissionDate).toISOString(),
        referencesId: [1]
      })
    }
    }>
      <table className={styles.gostTable}>
        <thead>
          <tr>
            <td>Поле</td>
            <td>Значение</td>
          </tr>
        </thead>
        <tbody>
          <tr>
              <td>Наименование стандарта</td>
              <td>
                <Input type='text' 
                  value={newGost.designation} 
                  onChange={(value: string) => setNewGost({...newGost, designation:value})}
                />
              </td>
          </tr>
          <tr>
              <td>Заглавие стандарта</td>
              <td>
                <Input type='text' 
                  value={newGost.fullName} 
                  onChange={(value: string) => setNewGost({...newGost, fullName:value})}
                />
              </td>
          </tr>
          <tr>
              <td>Код ОКС</td>
              <td>
              <Input type='text' 
                  value={newGost.codeOKS} 
                  onChange={(value: string) => setNewGost({...newGost, codeOKS:value})}
                />
              </td>
          </tr>
          <tr>
              <td>Сфера деятельности</td>
              <td>
                <TextArea  
                  value={newGost.activityField} 
                  onChange={(value: string) => setNewGost({...newGost, activityField:value})}
                />
              </td>
          </tr>
          <tr>
              <td>Год принятия</td>
              <td>
                <Input type='date' 
                  value={newGost.acceptanceDate} 
                  onChange={(value: string) => setNewGost({...newGost, acceptanceDate:value})}
                />
              </td>
          </tr>
          <tr>
              <td>Год введения</td>
              <td>
                <Input type='date'
                  value={newGost.commissionDate} 
                  onChange={(value: string) => setNewGost({...newGost, commissionDate:value})}
                />
              </td>
          </tr>
          <tr>
              <td>Разработчик</td>
              <td>
                <Input type='text' 
                  value={newGost.author} 
                  onChange={(value: string) => setNewGost({...newGost, author:value})}
                />
              </td>
          </tr>
          <tr>
              <td>Принят впервые/взамен</td>
              <td>
                <Input type='text' 
                  value={newGost.acceptedFirstTimeOrReplaced} 
                  onChange={(value: string) => setNewGost({...newGost, acceptedFirstTimeOrReplaced:value})}
                />
              </td>
          </tr>
          <tr>
              <td>Содержание</td>
              <td>
                <TextArea
                  value={newGost.content} 
                  onChange={(value: string) => setNewGost({...newGost, content:value})}
                />
              </td>
          </tr>
          <tr>
              <td>Область применения</td>
              <td>
                <TextArea 
                  value={newGost.applicationArea} 
                  onChange={(value: string) => setNewGost({...newGost, applicationArea:value})}
                />
              </td>
          </tr>
          <tr>
              <td>Ключевые слова</td>
              <td>
                <TextArea
                  value={newGost.keyWords} 
                  onChange={(value: string) => setNewGost({...newGost, keyWords:value})}
                />
              </td>
          </tr>
          <tr>
              <td>Ключевые фразы</td>
              <td>
                <TextArea 
                  value={newGost.keyPhrases} 
                  onChange={(value: string) => setNewGost({...newGost, keyPhrases:value})}
                />
              </td>
          </tr>
          <tr>
              <td>Уровень принятия</td>
              <td>
                <RadioGroup 
                  buttons={[
                    {id:'International', value:'0', label:'Международный'},
                    {id:'Foreign', value:'1', label:'Иностранный'},
                    {id:'Regional', value:'2', label:'Региональный'},
                    {id:'Organizational', value:'3', label:'Организационный'},
                    {id:'National', value:'4', label:'Национальный'},
                    {id:'Interstate', value:'5', label:'Межгосударственный'},
                  ]} 
                  name='adoptionLevel'
                  value={newGost.adoptionLevel.toString()}
                  onChange={(value: string) => {setNewGost({...newGost, adoptionLevel: parseInt(value)})}}
                />
              </td>
          </tr>
          <tr>
              <td>Текст стандарта</td>
              <td>
                <TextArea
                  value={newGost.documentText} 
                  onChange={(value: string) => setNewGost({...newGost, documentText:value})}
                />
              </td>
          </tr>
          <tr>
              <td>Нормативные ссылки</td>
              <td>
              <Input type='text' 
                  value={newGost.referencesId} 
                  onChange={(value: string) => setNewGost({...newGost, referencesId:parseInt(value)})}
                />
              </td>
          </tr>
          <tr>
              <td>Изменения</td>
              <td>
              <Input type='text' 
                  value={newGost.changes} 
                  onChange={(value: string) => setNewGost({...newGost, changes:value})}
                />
              </td>
          </tr>
          <tr>
              <td>Поправки</td>
              <td>
              <Input type='text' 
                  value={newGost.amendments} 
                  onChange={(value: string) => setNewGost({...newGost, amendments:value})}
                />
              </td>
          </tr>
          <tr>
              <td>Отменен/Заменен/Действующий</td>
              <td>
                <RadioGroup 
                  buttons={[
                    {id:'Cancelled', value:'0', label:'Отменем'},
                    {id:'Replaced', value:'1', label:'Замененный'},
                    {id:'Current', value:'2', label:'Действующий'},
                  ]} 
                  name='status'
                  value={newGost.status.toString()}
                  onChange={(value: string) => {setNewGost({...newGost, status: parseInt(value)})}}
                />
              </td>
          </tr>
          <tr>
              <td>Уровень гармонизации</td>
              <td className={styles.radioButtons}>
                <RadioGroup 
                  buttons={[
                    {id:'unharmonized', value:'0', label:'Негармонизированный'},
                    {id:'harmonized', value:'1', label:'Гармонизорованный'},
                    {id:'modified', value:'2', label:'Модифицированный'},
                  ]} 
                  name='harmonization'
                  value={newGost.harmonization.toString()}
                  onChange={(value: string) => {setNewGost({...newGost, harmonization: parseInt(value)})}}
                />
              </td>
          </tr>
        </tbody>
      </table>
      <Button type='submit' onClick={() => {}} isFilled className={styles.saveButton}>Сохранить</Button>
    </form>
  )
}

export default GostForm