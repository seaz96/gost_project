import axios from 'axios'
import React from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { Filter } from 'widgets/filter'
import { GostForm, newGostModel } from 'widgets/gost-form'

import styles from './GostEditPage.module.scss'
import { useAxios } from 'shared/hooks'
import { gostModel } from 'entities/gost'
import classNames from 'classnames'

const GostEditPage = () => {
  const navigate = useNavigate()
  const id = useParams().id
  const {response, loading, error} = useAxios<gostModel.Gost>(`localhost:8080/api/docs/${id}`)

  const editOldDocument = (gost: newGostModel.GostToSave) => {
    axios.put(`localhost:8080/api/docs/update/${id}`, gost, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('jwt_token')}`
      }
    })
    .then(response => navigate('/gost-review/' + id))
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
                referencesId: response.references.map(reference => reference.docId)
              }
            }
          />
        </section>
      </div>
    ) 
  else return <></>
}

export default GostEditPage;