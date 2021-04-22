import { useState } from 'react';

export const SignupForm = (props) => {
    const [form, setForm] = useState({});
    const [error, setError] = useState('');
    const onChange = (e, { name, value }) => {
        setForm({ ...form, [name]: value });
        //console.log('name', name);
    };

    const pepe = (event) => {
        setError('');
    };

    const onSubmit = (event) => {
        if (
            !form.firstName?.length ||
            !form.lastName?.length ||
            !form.email?.length ||
            !form.password?.length ||
            !form.passwordRe?.length
        ) {
            setError('Please fill the every blank');
            return;
        }
        for (var i = 0; i < form.email.length; i++)
            if (form.email[i] === '@' && i + 1 < form.email.length && form.email.indexOf('bilkent', i + 1) === -1) {
                setError('Please use your bilkent email');
                console.log(error);
                return;
            }

        if (form.password !== form.passwordRe) {
            setError('Passwords dont match');
            return;
        }

        props.history.push('/login');

        console.log('pepe');
    };
    return { form, onChange, onSubmit, error, pepe };
};
