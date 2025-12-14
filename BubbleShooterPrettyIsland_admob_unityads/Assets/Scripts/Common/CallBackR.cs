
public delegate R CallBackR<R>();
public delegate R CallBackR<R, T>(T t);
public delegate R CallBackR<R, T, T1>(T t, T1 t1);
public delegate R CallBackR<R, T, T1, T2>(T t, T1 t1, T2 t2);
public delegate R CallBackR<R, T, T1, T2, T3>(T t, T1 t1, T2 t2, T3 t3);
