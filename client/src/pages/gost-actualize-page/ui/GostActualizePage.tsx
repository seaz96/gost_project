import classNames from 'classnames'
import React from 'react'
import { GostForm, newGostModel } from 'widgets/gost-form'

import styles from './GostActualizePage.module.scss'
import axios from 'axios'
import { useNavigate, useParams } from 'react-router-dom'
import { useAxios } from 'shared/hooks'
import { gostModel } from 'entities/gost'

const GostActualizePage = () => {
  const id = useParams().id
  const {response, loading, error} = useAxios<gostModel.Gost>(`https://backend-seaz96.kexogg.ru/api/docs/${id}`)
  const navigate = useNavigate()

  const addNewDocument = (gost: newGostModel.GostToSave) => {
    console.log(gost)
    axios.put('https://backend-seaz96.kexogg.ru/api/docs/actualize/' + id, gost, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('jwt_token')}`
      },
      params: {
        docId: id
      }
    })
    .then(() => navigate('/gost-review/'+id))
  }

  if(loading) return <></>
  console.log(response)
  if(response)
  return (
    <div className='container'>
      <section className={classNames('contentContainer', styles.reviewSection)}>
        <h2>Актуализировать данные</h2>
        <GostForm handleSubmit={addNewDocument} gost={
          {
            ...response.primary,
            acceptanceDate: response.primary.acceptanceDate.split('T')[0],
            commissionDate: response.primary.commissionDate.split('T')[0],
            referencesId: response.references.map(reference => reference.docId)
          }
        }/>
      </section>
    </div>
  ) 
  else return <></>
}

export default GostActualizePage;