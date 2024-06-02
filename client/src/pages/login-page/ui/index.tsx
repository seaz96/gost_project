import React, {useContext, useState} from 'react'
import axios from 'axios';

import { AuthorizationForm, authorizationModel } from 'widgets/authorization-form';
import { RegistrationForm, registrationModel } from 'widgets/registration-form';
import urfuLogo from 'shared/assets/urfu.png';
import styles from './LoginPage.module.scss';
import { UserContext, userModel } from 'entities/user';
import { axiosInstance } from 'shared/configs/axiosConfig';

enum states {
    authorization,
    registration
}

const LoginPage = () => {
    const [state, setState] = useState<states>(states.registration)
    const {setUser} = useContext(UserContext)

    const handleRegistration = (user: registrationModel.UserRegistration) => {
        axiosInstance.post<userModel.User>('/accounts/register', user)
        .then(response => {
            setUser(response.data);
            localStorage.setItem('jwt_token', response.data.token);
            window.location.href = "/";
        })
        .catch(error => console.log(error))
    }

    const handleAuthorization = (user: authorizationModel.UserAuthorization) => {
        return axiosInstance.post<userModel.User>('/accounts/login', user)
        .then((response) => {
            setUser(response.data);
            localStorage.setItem('jwt_token', response.data.token);
            window.location.href = "/";
            return null
        })
    }

    return (
    <div className={styles.loginPageContainer}>
        <img className={styles.logo} src={urfuLogo} alt='logo'/>
        {
            state === states.authorization 
            ? 
            <section className={styles.authorizationForm}>
                <AuthorizationForm changeForm={() => setState(states.registration)} onSubmit={(user: authorizationModel.UserAuthorization) => handleAuthorization(user)}/>
            </section>
            :
            <section className={styles.registrationForm}>
                <RegistrationForm changeForm={() => setState(states.authorization)} onSubmit={(user: registrationModel.UserRegistration) => handleRegistration(user)}/>
            </section>
        }
    </div>
    )
}

export default LoginPage;