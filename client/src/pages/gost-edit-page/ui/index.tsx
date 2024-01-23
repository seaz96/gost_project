import axios from 'axios'
import React from 'react'
import { useParams } from 'react-router-dom'
import { Filter } from 'widgets/filter'
import { GostForm, newGostModel } from 'widgets/gost-form'

import styles from './GostEditPage.module.scss'
import { useAxios } from 'shared/hooks'
import { gostModel } from 'entities/gost'
import classNames from 'classnames'

const GostEditPage = () => {
  const id = useParams().id
  const {response, loading, error} = useAxios<gostModel.Gost>(`https://backend-seaz96.kexogg.ru/api/docs/${id}`)

  const editOldDocument = (gost: newGostModel.GostToSave) => {
    axios.put(`https://backend-seaz96.kexogg.ru/api/docs/update/${id}`, gost, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('jwt_token')}`
      }
    })
    .then(response => console.log(response))
  }

  if(loading) return <></>

  if(response) 
    return (
      <div className='container'>
        <section className={classNames('contentContainer', styles.reviewSection)}>
          <GostForm 
            handleSubmit={editOldDocument} 
            gost={
              {
                ...response.primary,
                acceptanceDate: response.primary.acceptanceDate.split('T')[0],
                commissionDate: response.primary.commissionDate.split('T')[0],
              }
            }
          />
        </section>
      </div>
    ) 
  else return <></>
}

export default GostEditPage;