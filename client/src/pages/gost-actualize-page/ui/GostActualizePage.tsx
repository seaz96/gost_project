import classNames from 'classnames'
import React from 'react'
import { GostForm, newGostModel } from 'widgets/gost-form'

import styles from './GostActualizePage.module.scss'
import axios from 'axios'
import { useNavigate, useParams } from 'react-router-dom'
import { useAxios } from 'shared/hooks'
import { gostModel } from 'entities/gost'
import { axiosInstance } from 'shared/configs/axiosConfig'

const GostActualizePage = () => {
  const id = useParams().id
  const {response, loading, error} = useAxios<gostModel.Gost>(`/docs/${id}`)
  const navigate = useNavigate()

  const addNewDocument = (gost: newGostModel.GostToSave, file: File) => {
    axiosInstance.put('/docs/actualize/' + id, gost, {
      params: {
        docId: id
      }
    }
    )
    .then(response => handleUploadFile(file, id))
    .then(responce => navigate('/gost-review/'+id))
  }

  const handleUploadFile = (file: File, docId: string | undefined) => {
    axiosInstance({
      method: 'post',
      url: `/docs/${docId}/upload-file`,
      data: {
        File: file,
        Extension: file.name.split('.').pop()
      },
      headers: {"Content-Type": "multipart/form-data"}
    })
  }

  if(loading) return <></>
  console.log(response)
  if(response)
  return (
    <div className='container'>
      <section className={classNames('contentContainer', styles.reviewSection)}>
        <h2>Актуализировать данные</h2>
        <GostForm handleUploadFile={handleUploadFile} handleSubmit={addNewDocument} gost={
          {
            ...response.primary,
            references: response.references.map(reference => reference.designation)
          }
        }/>
      </section>
    </div>
  ) 
  else return <></>
}

export default GostActualizePage;