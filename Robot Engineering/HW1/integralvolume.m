syms q1 q2 q3 real
syms a1 a2 a3 d1 real

d1=2;
a1=0.3;
a2=1;
a3=0.8;

px = expand(cos(q1)*(a1+a2*cos(q2)+a3*cos(q2+q3)));
py = expand(sin(q1)*(a1+a2*cos(q2)+a3*cos(q2+q3)));
pz = expand(d1 + a2*sin(q2) + a3*sin(q2 + q3));


J = [diff(px, q1), diff(px, q2), diff(px, q3);
diff(py, q1), diff(py, q2), diff(py, q3);
diff(pz, q1), diff(pz, q2), diff(pz, q3)];

disp(simplify(det(J)));

q1min = -150*pi/180; q1max = 150*pi/180;
q2min = -140*pi/180; q2max = 140*pi/180;
q3min = -100*pi/180; q3max = 100*pi/180;

