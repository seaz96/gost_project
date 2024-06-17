import axios from 'axios'
import React from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { Filter } from 'widgets/filter'
import { GostForm, newGostModel } from 'widgets/gost-form'

import styles from './GostEditPage.module.scss'
import { useAxios } from 'shared/hooks'
import { gostModel } from 'entities/gost'
import classNames from 'classnames'
import { axiosInstance } from 'shared/configs/axiosConfig'
import { Extension } from 'typescript'

const GostEditPage = () => {
  const navigate = useNavigate()
  const id = useParams().id
  const {response, loading, error} = useAxios<gostModel.Gost>(`/docs/${id}`)

  const editOldDocument = (gost: newGostModel.GostToSave, file: File) => {
    axiosInstance.put(`/docs/update/${id}`, gost)
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

  if(response) 
    return (
      <div className='container'>
        <section className={classNames('contentContainer', styles.reviewSection)}>
          <GostForm
            handleUploadFile={handleUploadFile}
            handleSubmit={editOldDocument} 
            gost={
              {
                ...response.primary,
                references: response.references.map(reference => reference.designation)
              }
            }
          />
        </section>
      </div>
    ) 
  else return <></>
}

export default GostEditPage;