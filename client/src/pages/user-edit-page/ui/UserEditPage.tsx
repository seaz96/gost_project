import React from 'react'

import styles from './UserEditPage.module.scss'
import { UserEditForm } from 'widgets/user-edit-form';
import { useAxios } from 'shared/hooks';
import { userModel } from 'entities/user';
import { useParams } from 'react-router-dom';


const UserEditPage = () => {
    const id = useParams().id
    const {response, loading, error} = useAxios<userModel.User>(`https://backend-seaz96.kexogg.ru/api/acc
    ounts/${id}`)

    if(loading) return <></>
    
    if(response) 
    return (
        <div>
            <section className={styles.userEditSection}>
                <UserEditForm handleSubmit={() => {}} userData={response}/>
            </section>
        </div>
    )
    else return <></>
}

export default UserEditPage;